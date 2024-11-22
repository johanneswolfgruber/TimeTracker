type ButtonProps = {
  onClick?: () => void;
  isReadOnly?: boolean;
  children: React.ReactNode;
};

export const IconButton = ({ onClick, isReadOnly, children }: ButtonProps) => {
  return (
    <button
      onClick={onClick}
      disabled={isReadOnly}
      className="p-2 bg-blue hover:bg-text text-base outline-none rounded-full drop-shadow-lg disabled:opacity-50 disabled:hover:bg-blue"
      type="button"
    >
      {children}
    </button>
  );
};

export const TextButton = ({ onClick, children }: ButtonProps) => {
  return (
    <button
      onClick={onClick}
      className="px-4 py-2 bg-surface1 hover:bg-text hover:text-base tracking-wider outline-none rounded-lg drop-shadow-lg"
      type="button"
    >
      {children}
    </button>
  );
};

export const FormButton = ({ onClick, children }: ButtonProps) => {
  return (
    <button
      onClick={onClick}
      className="px-4 py-2 bg-blue hover:bg-text text-base hover:text-base tracking-wider outline-none rounded-lg drop-shadow-lg"
      type="submit"
    >
      {children}
    </button>
  );
};
