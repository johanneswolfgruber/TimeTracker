import { TimeSpan } from "./TimeSpan";

export class RunningDurations {
  runningDurationsById: { [key: string]: TimeSpan } = {};

  public updateRunningDuration(id: string, duration: TimeSpan) {
    this.runningDurationsById[id] = duration;
  }

  public getRunningDuration(id: string): TimeSpan {
    return this.runningDurationsById[id] || new TimeSpan();
  }

  public getRunningDurationSum(ids: string[]): TimeSpan {
    return ids.reduce(
      (acc, id) => acc.add(this.getRunningDuration(id)),
      new TimeSpan(),
    );
  }

  public getTotalRunningDurations(): TimeSpan {
    return Object.values(this.runningDurationsById).reduce(
      (acc, duration) => acc.add(duration),
      new TimeSpan(),
    );
  }

  public static copyFrom(runningDurations: RunningDurations) {
    const newRunningDurations = new RunningDurations();
    newRunningDurations.runningDurationsById = {
      ...runningDurations.runningDurationsById,
    };
    return newRunningDurations;
  }
}
