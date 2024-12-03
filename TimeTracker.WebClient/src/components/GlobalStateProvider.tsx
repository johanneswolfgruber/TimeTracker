import { useCallback, useEffect, useState } from "react";
import { TimeSpan } from "../utils/TimeSpan";
import { RunningDurations } from "../utils/RunningDurations";
import { IActivity } from "../lib/types";
import { useAuthentication } from "../context/AuthenticationContext";
import { fetchActivities } from "../lib/api";
import { GlobalStateContext } from "../context/GlobalStateContext";

type GlobalStateProviderProps = {
  children: React.ReactNode;
};

export const GlobalStateProvider = ({ children }: GlobalStateProviderProps) => {
  const [activities, setActivities] = useState<IActivity[]>([]);
  const [selectedActivityId, setSelectedActivityId] = useState<string | null>(
    activities.length > 0 ? activities[0].id : null,
  );
  const [runningDurations, setRunningDurations] = useState<RunningDurations>(
    new RunningDurations(),
  );
  const auth = useAuthentication();

  const getCurrentlyRunningDurations = useCallback(
    (activityId: string | null) => {
      const selectedActivity = activities.find((a) => a.id === activityId);
      const trackings = selectedActivity?.trackings || [];
      const currentlyRunning = new RunningDurations();

      trackings.forEach((tracking) => {
        if (tracking.endTime) {
          return;
        }

        const duration = Math.floor(
          (new Date().getTime() - new Date(tracking.startTime).getTime()) /
            1000,
        );

        currentlyRunning.updateRunningDuration(
          tracking.id,
          TimeSpan.fromSeconds(duration),
        );
      });

      return currentlyRunning;
    },
    [activities],
  );

  useEffect(() => {
    let ignore = false;

    const fetch = async () => {
      if (ignore || !auth?.user?.userId) {
        return;
      }

      const res = await fetchActivities(auth.user?.userId);
      if (!res.isSuccess || !res.value) {
        return;
      }

      setActivities(res.value);
      if (res.value.length > 0) {
        setSelectedActivityId(res.value[0].id);
      }
    };

    fetch().catch(console.error);

    return () => {
      ignore = true;
    };
  }, [auth?.user?.userId]);

  useEffect(() => {
    const interval = setInterval(() => {
      const currentlyRunningDurations =
        getCurrentlyRunningDurations(selectedActivityId);

      if (!Object.keys(currentlyRunningDurations.runningDurationsById).length) {
        return;
      }

      setRunningDurations(getCurrentlyRunningDurations(selectedActivityId));
    }, 1000);

    return () => {
      clearInterval(interval);
    };
  }, [activities, selectedActivityId, getCurrentlyRunningDurations]);

  const reloadActivities = async () => {
    if (!auth?.user?.userId) {
      return;
    }

    const res = await fetchActivities(auth.user?.userId);
    if (!res.isSuccess || !res.value) {
      return;
    }

    setActivities(res.value);
    if (
      !selectedActivityId ||
      !res.value.find((a) => a.id === selectedActivityId)
    ) {
      setSelectedActivityId(res.value.length > 0 ? res.value[0].id : null);
    }
  };

  const handleSelectedActivityChanged = (activityId: string) => {
    setSelectedActivityId(activityId);
    setRunningDurations(getCurrentlyRunningDurations(activityId));
  };

  return (
    <GlobalStateContext.Provider
      value={{
        activities,
        selectedActivityId,
        runningDurations,
        reloadActivities,
        handleSelectedActivityChanged,
      }}
    >
      {children}
    </GlobalStateContext.Provider>
  );
};
