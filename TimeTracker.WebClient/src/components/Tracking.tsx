import {
  PencilSquareIcon,
  StopIcon,
  TrashIcon,
} from "@heroicons/react/24/solid";
import { ITracking, ITrackingFormData } from "../lib/types";
import { deleteTracking, updateTracking } from "../lib/api";
import { IconButton } from "./Buttons";
import { useState } from "react";
import { TrackingsDetail } from "./TrackingsDetail";
import { TimeSpan } from "../utils/TimeSpan";
import { useGlobalState } from "../context/GlobalStateContext";

type TrackingProps = {
  tracking: ITracking;
};

export const Tracking = ({ tracking }: TrackingProps) => {
  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false);
  const globalState = useGlobalState();

  const handleStopTracking = async () => {
    const newTracking = {
      ...tracking,
      endTime: new Date(new Date().toISOString()),
    };
    await updateTracking(newTracking);
    await globalState?.reloadActivities();
  };

  const handleDeleteTracking = async () => {
    await deleteTracking(tracking.id);
    await globalState?.reloadActivities();
  };

  const handleEditTracking = () => {
    setIsEditModalOpen(true);
  };

  const handleUpdateTracking = async (trackingData: ITrackingFormData) => {
    const updatedTracking = {
      ...tracking,
      startTime: trackingData.startTime,
      endTime: trackingData.endTime,
      notes: trackingData.notes,
    };
    await updateTracking(updatedTracking);
    setIsEditModalOpen(false);
    await globalState?.reloadActivities();
  };

  return (
    <>
      <tr className="border-b border-surface1 last:border-b-0">
        <td className="p-2 text-left">
          {new Date(tracking.startTime).toLocaleDateString()}
        </td>
        <td className="p-2 text-left">
          {new Date(tracking.startTime).toLocaleTimeString()}
        </td>
        <td className="p-2 text-left">
          {tracking.endTime && new Date(tracking.endTime).toLocaleTimeString()}
        </td>
        <td className="p-2 text-left">
          {tracking.endTime
            ? TimeSpan.parse(tracking.duration).toString()
            : globalState?.runningDurations
                .getRunningDuration(tracking.id)
                .toString()}
        </td>
        <td className="p-2 text-left text-ellipsis overflow-hidden whitespace-nowrap">
          {tracking.notes}
        </td>
        <td className="p-2">
          <div className="flex flex-row justify-end gap-2">
            {!tracking.endTime && (
              <IconButton onClick={handleStopTracking}>
                <StopIcon
                  className={`w-4 h-4 ${!tracking.endTime && "animate-pulse"}`}
                />
              </IconButton>
            )}
            <IconButton onClick={handleEditTracking}>
              <PencilSquareIcon className="w-4 h-4" />
            </IconButton>
            <IconButton onClick={handleDeleteTracking}>
              <TrashIcon className="w-4 h-4" />
            </IconButton>
            <TrackingsDetail
              isModalOpen={isEditModalOpen}
              onSubmit={handleUpdateTracking}
              onClose={() => setIsEditModalOpen(false)}
              initialFormData={{
                startTime: tracking.startTime,
                endTime: tracking.endTime,
                duration:
                  globalState?.runningDurations.getRunningDuration(
                    tracking.id,
                  ) || new TimeSpan(),
                notes: tracking.notes,
              }}
            />
          </div>
        </td>
      </tr>
    </>
  );
};
