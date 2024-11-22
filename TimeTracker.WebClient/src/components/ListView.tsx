import { IActivity } from "../lib/types";
import { ActivityList } from "./ActivityList";
import { TrackingsOverview } from "./TrackingsOverview";
import { useGlobalState } from "../context/GlobalStateContext";

export const ListView = () => {
  const globalState = useGlobalState();

  const selectedActivity = globalState?.activities.find(
    (a) => a.id === globalState?.selectedActivityId,
  );

  return (
    <>
      <div className="mt-16 mx-auto p-4 w-full grid grid-cols-4 gap-4">
        <div className="bg-surface0 rounded-lg shadow-xl">
          <ActivityList
            activities={globalState?.activities ?? []}
            selectedActivityId={globalState?.selectedActivityId || null}
            onSelectedActivityChanged={(activity: IActivity) =>
              globalState?.handleSelectedActivityChanged(activity.id)
            }
            onActivityAdded={async () => await globalState?.reloadActivities()}
            onActivityChanged={async () => globalState?.reloadActivities()}
          />
        </div>
        <div className="relative col-span-3 bg-surface0 rounded-lg shadow-xl">
          <TrackingsOverview trackings={selectedActivity?.trackings ?? []} />
        </div>
      </div>
    </>
  );
};
