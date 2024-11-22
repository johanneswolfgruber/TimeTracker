import {
  PencilSquareIcon,
  PlayIcon,
  TrashIcon,
} from "@heroicons/react/24/solid";
import { deleteActivity, startTracking, updateActivityName } from "../lib/api";
import { IActivity } from "../lib/types";
import { FormButton, IconButton } from "./Buttons";
import { ModalDialog } from "./ModalDialog";
import { useState } from "react";

type ActivityProps = {
  activity: IActivity;
  isSelected: boolean;
  onActivityChanged: (activityId: string) => Promise<void>;
};

export const Activity = ({
  activity,
  isSelected,
  onActivityChanged,
}: ActivityProps) => {
  const [activityName, setActivityName] = useState(activity.name);
  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false);

  const handleActivityNameChanged = (
    e: React.ChangeEvent<HTMLInputElement>,
  ) => {
    setActivityName(e.target.value);
  };

  const handleStartTracking = async () => {
    await startTracking(activity.id);
    await onActivityChanged(activity.id);
  };

  const handleDeleteActivity = async () => {
    await deleteActivity(activity.id);
    await onActivityChanged(activity.id);
  };

  const handleEditActivity = () => {
    setIsEditModalOpen(true);
  };

  const handleUpdateActivityName = async () => {
    await updateActivityName({
      ...activity,
      name: activityName,
    });
    setIsEditModalOpen(false);
    await onActivityChanged(activity.id);
  };

  return (
    <>
      <div
        className={`px-4 py-2 flex flex-row justify-between items-center ${isSelected ? "bg-surface1" : "bg-surface0"} cursor-pointer hover:bg-surface1 rounded-lg shadow-xl ${isSelected ? "border-blue" : "border-surface1"} border-2`}
      >
        <p className="font-bold tracking-wider">{activity.name}</p>
        <div className="grid grid-cols-3 gap-2">
          <IconButton onClick={handleStartTracking}>
            <PlayIcon className="w-4 h-4" />
          </IconButton>
          <IconButton onClick={handleEditActivity}>
            <PencilSquareIcon className="w-4 h-4" />
          </IconButton>
          <IconButton onClick={handleDeleteActivity}>
            <TrashIcon className="w-4 h-4" />
          </IconButton>
        </div>
      </div>

      <ModalDialog
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
      >
        <div className="p-4">
          <h2 className="mb-4 text-left text-lg font-bold">Edit Activity</h2>
          <form
            onSubmit={handleUpdateActivityName}
            className="flex flex-col justify-evenly items-start"
          >
            <label htmlFor="activityName">Name</label>
            <input
              className="mb-4 w-full bg-transparent border-b border-text outline-none"
              placeholder="Activity Name"
              type="text"
              name="activityName"
              id="activityName"
              value={activityName}
              onChange={handleActivityNameChanged}
            />
            <FormButton>Update</FormButton>
          </form>
        </div>
      </ModalDialog>
    </>
  );
};
