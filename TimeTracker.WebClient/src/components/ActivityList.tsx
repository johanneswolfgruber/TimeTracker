import { useState } from "react";
import { IActivity } from "../lib/types";
import { PlusIcon } from "@heroicons/react/24/solid";
import { createActivity } from "../lib/api";
import { IconButton } from "./Buttons";
import { Activity } from "./Activity";
import { useAuthentication } from "../context/AuthenticationContext";

type ActivityListProps = {
  activities: IActivity[];
  selectedActivityId: string | null;
  onSelectedActivityChanged: (activity: IActivity) => void;
  onActivityAdded: () => Promise<void>;
  onActivityChanged: (activityId: string) => Promise<void>;
};

export const ActivityList = ({
  activities,
  selectedActivityId,
  onSelectedActivityChanged,
  onActivityAdded,
  onActivityChanged,
}: ActivityListProps) => {
  const [newActivityName, setNewActivityName] = useState("");
  const auth = useAuthentication();

  const handleAddActivity = async () => {
    if (!auth?.user?.userId) {
      console.error("No user id");
      return;
    }

    await createActivity(newActivityName, auth.user.userId);
    setNewActivityName("");
    await onActivityAdded();
  };

  return (
    <div className="p-4">
      <h2 className="text-left text-lg font-bold">Activities</h2>
      <div className="my-4 px-4 py-2 flex flex-row justify-between items-center cursor-pointer bg-surface0 rounded-lg shadow-xl border-surface1 border-2">
        <input
          className="bg-surface0 border-b border-text focus:outline-none"
          placeholder="New Activity"
          id="new-activity-name"
          type="text"
          value={newActivityName}
          onChange={(e) => setNewActivityName(e.target.value)}
        />
        <IconButton
          onClick={handleAddActivity}
          isReadOnly={newActivityName === ""}
        >
          <PlusIcon className="w-4 h-4" />
        </IconButton>
      </div>
      {activities.map((activity: IActivity) => {
        return (
          <div
            className="my-3"
            key={activity.id}
            onClick={() => onSelectedActivityChanged(activity)}
          >
            <Activity
              activity={activity}
              isSelected={activity.id === selectedActivityId}
              onActivityChanged={onActivityChanged}
            />
          </div>
        );
      })}
    </div>
  );
};
