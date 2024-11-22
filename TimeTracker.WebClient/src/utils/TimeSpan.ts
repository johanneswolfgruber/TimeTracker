export class TimeSpan {
  hours: number;
  minutes: number;
  seconds: number;

  constructor(hours: number = 0, minutes: number = 0, seconds: number = 0) {
    this.hours = hours;
    this.minutes = minutes;
    this.seconds = seconds;
  }

  public getTotalSeconds(): number {
    return this.hours * 3600 + this.minutes * 60 + this.seconds;
  }

  public add(timeSpan: TimeSpan): TimeSpan {
    return TimeSpan.fromSeconds(
      this.getTotalSeconds() + timeSpan.getTotalSeconds(),
    );
  }

  public toString(): string {
    return `${this.hours.toString()}:${this.minutes.toString().padStart(2, "0")}:${this.seconds.toString().padStart(2, "0")}`;
  }

  public static fromSeconds(totalSeconds: number): TimeSpan {
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds / 60) % 60);
    const seconds = Math.floor(totalSeconds % 60);
    return new TimeSpan(hours, minutes, seconds);
  }

  public static parse(timeSpan: string): TimeSpan {
    if (timeSpan === null || timeSpan === undefined || timeSpan === "") {
      return new TimeSpan();
    }

    const parts = timeSpan.split(".")[0].split(":");
    const hours = parseInt(parts[0]);
    const minutes = parseInt(parts[1]);
    const seconds = parseInt(parts[2]);
    if (isNaN(hours) || isNaN(minutes) || isNaN(seconds)) {
      return new TimeSpan();
    }

    return new TimeSpan(hours, minutes, seconds);
  }
}
