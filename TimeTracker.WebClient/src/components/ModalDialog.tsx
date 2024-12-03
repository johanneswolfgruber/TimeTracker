import { useEffect, useRef, useState } from "react";

export type ModalProps = {
  isOpen: boolean;
  hasCloseBtn?: boolean;
  onClose?: () => void;
  children: React.ReactNode;
};

export const ModalDialog = (props: ModalProps) => {
  const [isModalOpen, setIsModalOpen] = useState(props.isOpen);
  const ref = useRef<HTMLDialogElement | null>(null);

  useEffect(() => {
    setIsModalOpen(props.isOpen);
  }, [props.isOpen]);

  useEffect(() => {
    const modalElement = ref.current;
    if (modalElement) {
      if (isModalOpen) {
        modalElement.showModal();
      } else {
        modalElement.close();
      }
    }
  }, [isModalOpen]);

  const handleCloseModal = () => {
    if (props.onClose) {
      props.onClose();
    }
    setIsModalOpen(false);
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLDialogElement>) => {
    if (event.key === "Escape") {
      handleCloseModal();
    }
  };

  return (
    <dialog
      className="p-4 rounded-lg bg-surface0 text-text shadow-xl backdrop-blur-xl"
      ref={ref}
      onKeyDown={handleKeyDown}
    >
      {props.hasCloseBtn && (
        <button className="modal-close-btn" onClick={handleCloseModal}>
          Close
        </button>
      )}
      {props.children}
    </dialog>
  );
};
