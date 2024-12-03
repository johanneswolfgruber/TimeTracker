import { createContext, useContext } from "react";
import { RunningDurations } from "../utils/RunningDurations";
import { IActivity } from "../lib/types";

type GlobalStateContextData = {
  activities: IActivity[];
  selectedActivityId: string | null;
  runningDurations: RunningDurations;
  reloadActivities: () => Promise<void>;
  handleSelectedActivityChanged: (activityId: string) => void;
};

export const GlobalStateContext = createContext<GlobalStateContextData | null>(
  null,
);

export const useGlobalState = () => {
  return useContext(GlobalStateContext);
};
