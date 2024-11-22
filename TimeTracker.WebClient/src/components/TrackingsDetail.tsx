import { useEffect, useState } from "react";
import { ITrackingFormData } from "../lib/types";
import { ModalDialog } from "./ModalDialog";
import { FormButton } from "./Buttons";

type TrackingsDetailProps = {
  initialFormData: ITrackingFormData;
  isModalOpen: boolean;
  onSubmit: (date: ITrackingFormData) => Promise<void>;
  onClose?: () => void;
};

export const TrackingsDetail = ({
  initialFormData,
  isModalOpen,
  onSubmit,
  onClose,
}: TrackingsDetailProps) => {
  const [date, setDate] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [notes, setNotes] = useState("");

  useEffect(() => {
    setDate(
      new Intl.DateTimeFormat("en-CA", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
      }).format(new Date(initialFormData.startTime)),
    );
    setStartTime(new Date(initialFormData.startTime).toLocaleTimeString());
    setEndTime(
      initialFormData.endTime
        ? new Date(initialFormData.endTime).toLocaleTimeString()
        : "",
    );
    setNotes(initialFormData.notes || "");
  }, [initialFormData]);

  const handleDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setDate(event.target.value);
  };

  const handleTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;

    if (name === "startTime") {
      setStartTime(value);
    } else if (name === "endTime") {
      setEndTime(value);
    }
  };

  const handleNotesChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    setNotes(event.target.value);
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = {
      startTime: new Date(`${date}T${startTime}`),
      endTime: !endTime ? undefined : new Date(`${date}T${endTime}`),
      duration: initialFormData.duration,
      notes,
    };
    onSubmit(formData);
  };

  return (
    <ModalDialog isOpen={isModalOpen} onClose={onClose}>
      <form className="p-4 min-w-[300px]" onSubmit={handleSubmit}>
        <h2 className="mb-4 text-left text-lg font-bold">Details</h2>
        <div className="flex flex-col justify-evenly items-start bg-surface0 rounded-lg space-y-4">
          <div className="w-full grid grid-cols-2 gap-8">
            <section className="mr-8 flex flex-col gap-2 items-start">
              <label htmlFor="date">Date</label>
              <input
                className="w-full bg-transparent text-text outline-none border-b border-text"
                value={date}
                onChange={handleDateChange}
                type="date"
                name="date"
                id="date"
              />
            </section>
            <section className="flex flex-col gap-2 items-start">
              <label htmlFor="duration">Duration</label>
              <input
                className="w-full bg-transparent text-text outline-none"
                value={initialFormData?.duration.toString()}
                type="text"
                name="duration"
                id="duration"
                readOnly
              />
            </section>
          </div>
          <div className="w-full grid grid-cols-2 gap-8">
            <section className="flex flex-col gap-2 items-start">
              <label htmlFor="startTime">Start Time</label>
              <input
                className="w-full bg-transparent text-text outline-none"
                value={startTime}
                onChange={handleTimeChange}
                type="time"
                step="1"
                name="startTime"
                id="startTime"
              />
            </section>
            <section className="flex flex-col gap-2 items-start">
              <label htmlFor="endTime">End Time</label>
              <input
                className="w-full bg-transparent text-text outline-none"
                value={endTime}
                onChange={handleTimeChange}
                type="time"
                step="1"
                name="endTime"
                id="endTime"
              />
            </section>
          </div>
          <section className="w-full flex flex-col gap-2 items-start">
            <label htmlFor="notes">Notes</label>
            <textarea
              className="w-full p-2 bg-transparent text-text outline-none border-text border-2 rounded-lg max-h-32 min-h-12"
              value={notes}
              onChange={handleNotesChange}
              name="notes"
              id="notes"
              placeholder="Notes"
            />
          </section>
          <FormButton>Update</FormButton>
        </div>
      </form>
    </ModalDialog>
  );
};
