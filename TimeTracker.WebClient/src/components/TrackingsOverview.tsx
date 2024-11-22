import { ITracking } from "../lib/types";
import { Tracking } from "./Tracking";
import { Expander } from "./Expander";
import { getWeekNumber } from "../utils/helpers";
import { TimeSpan } from "../utils/TimeSpan";
import { useGlobalState } from "../context/GlobalStateContext";

type TrackingsOverviewProps = {
  trackings: ITracking[];
};

export const TrackingsOverview = ({ trackings }: TrackingsOverviewProps) => {
  const globalState = useGlobalState();

  const groupedByWeek = trackings.reduce(
    (acc, tracking) => {
      const weekNumber = getWeekNumber(new Date(tracking.startTime))[1];
      if (!acc[weekNumber]) {
        acc[weekNumber] = [];
      }
      acc[weekNumber].push(tracking);
      return acc;
    },
    {} as { [key: number]: ITracking[] },
  );

  return (
    <div className="p-4">
      <h2 className="mb-4 text-left text-lg font-bold">
        Trackings{" | "}
        {trackings
          .reduce(
            (acc, tracking) => acc.add(TimeSpan.parse(tracking.duration)),
            new TimeSpan(),
          )
          .add(
            globalState?.runningDurations.getRunningDurationSum(
              trackings.filter((x) => !x.endTime).map((x) => x.id),
            ) || new TimeSpan(),
          )
          .toString()}
      </h2>
      {Object.keys(groupedByWeek).map((weeknumber) => (
        <div key={weeknumber} className="mb-4">
          <Expander
            header={`KW ${weeknumber}, ${groupedByWeek[Number(weeknumber)]
              .reduce(
                (acc, tracking) => acc.add(TimeSpan.parse(tracking.duration)),
                new TimeSpan(),
              )
              .add(
                globalState?.runningDurations.getRunningDurationSum(
                  groupedByWeek[Number(weeknumber)].map((x) => x.id),
                ) || new TimeSpan(),
              )
              .toString()}`}
          >
            <table className="w-full table-fixed border-collapse">
              <thead>
                <tr>
                  <th className="p-2 bg-surface1 text-left">Date</th>
                  <th className="p-2 bg-surface1 text-left">Start time</th>
                  <th className="p-2 bg-surface1 text-left">End time</th>
                  <th className="p-2 bg-surface1 text-left">Duration</th>
                  <th className="p-2 bg-surface1 text-left">Notes</th>
                  <th className="p-2 bg-surface1 text-left"></th>
                </tr>
              </thead>
              <tbody>
                {groupedByWeek[Number(weeknumber)].map(
                  (tracking: ITracking) => (
                    <Tracking key={tracking.id} tracking={tracking} />
                  ),
                )}
              </tbody>
            </table>
          </Expander>
        </div>
      ))}
    </div>
  );
};
