import { ChevronDownIcon, ChevronUpIcon } from "@heroicons/react/24/solid";
import { useState } from "react";

type ExpanderProps = {
  header: React.ReactNode;
  children: React.ReactNode;
};

export const Expander = ({ header, children }: ExpanderProps) => {
  const [isExpanded, setIsExpanded] = useState(false);

  const handleClick = () => {
    setIsExpanded(!isExpanded);
  };

  return (
    <div className="w-full border-2 border-surface1 rounded-lg shadow-xl">
      <button
        onClick={handleClick}
        className={`w-full h-12 hover:bg-surface1 flex flex-row justify-between items-center outline-none ${isExpanded ? "border-surface1" : "border-transparent"} border-b-2`}
      >
        <div className="ml-4 font-bold tracking-widest">{header}</div>
        <div className="mr-4">
          {isExpanded ? (
            <ChevronDownIcon className="w-6 h-6 font-bold" />
          ) : (
            <ChevronUpIcon className="w-6 h-6 font-bold" />
          )}
        </div>
      </button>
      <div>{isExpanded && children}</div>
    </div>
  );
};
