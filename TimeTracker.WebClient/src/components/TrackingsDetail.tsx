import { useEffect, useRef, useState } from "react";
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
  const [formData, setFormData] = useState(initialFormData);
  const dateRef = useRef<HTMLInputElement | null>(null);
  const startTimeRef = useRef<HTMLInputElement | null>(null);
  const endTimeRef = useRef<HTMLInputElement | null>(null);
  const notesRef = useRef<HTMLTextAreaElement | null>(null);
  const [duration, setDuration] = useState(initialFormData.duration);

  useEffect(() => {
    setDuration(initialFormData.duration);
  }, [initialFormData.duration]);

  const handleDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.target;

    setFormData((prevFormData) => {
      const startTimeElement = startTimeRef.current;
      const endTimeElement = endTimeRef.current;
      if (!startTimeElement || !endTimeElement) {
        return prevFormData;
      }

      const startTime = new Date(`${value}T${startTimeElement.value}`);
      console.log(startTime);
      const endTime = new Date(`${value}T${endTimeElement.value}`);
      console.log(endTime);

      return {
        ...prevFormData,
        startTime,
        endTime,
      };
    });
  };

  const handleTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;

    setFormData((prevFormData) => {
      const dateRefElement = dateRef.current;
      if (!dateRefElement) {
        return prevFormData;
      }

      const time = new Date(`${dateRefElement.value}T${value}`);
      console.log(time);

      return {
        ...prevFormData,
        [name]: time,
      };
    });
  };

  const handleNotesChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const { value } = event.target;
    setFormData((prevFormData) => ({
      ...prevFormData,
      notes: value,
    }));
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (formData) {
      onSubmit(formData);
    }
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
                ref={dateRef}
                value={new Intl.DateTimeFormat("en-CA", {
                  year: "numeric",
                  month: "2-digit",
                  day: "2-digit",
                }).format(new Date(formData.startTime))}
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
                value={duration.toString()}
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
                ref={startTimeRef}
                value={new Date(formData.startTime).toLocaleTimeString()}
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
                onChange={handleTimeChange}
                ref={endTimeRef}
                value={
                  formData.endTime
                    ? new Date(formData.endTime).toLocaleTimeString()
                    : ""
                }
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
              onChange={handleNotesChange}
              ref={notesRef}
              value={formData.notes}
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
