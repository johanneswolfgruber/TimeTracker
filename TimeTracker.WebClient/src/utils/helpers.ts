// https://stackoverflow.com/questions/6117814/get-week-of-year-in-javascript-like-in-php
export const getWeekNumber = (date: Date): number[] => {
  // Copy date so don't modify original
  date = new Date(
    Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()),
  );
  // Set to nearest Thursday: current date + 4 - current day number
  // Make Sunday's day number 7
  date.setUTCDate(date.getUTCDate() + 4 - (date.getUTCDay() || 7));
  // Get first day of year
  const yearStart = new Date(Date.UTC(date.getUTCFullYear(), 0, 1));
  // Calculate full weeks to nearest Thursday
  const weekNo = Math.ceil(
    ((date.getTime() - yearStart.getTime()) / 86400000 + 1) / 7,
  );
  // Return array of year and week number
  return [date.getUTCFullYear(), weekNo];
};

export const formatDuration = (duration: number): string => {
  const hours = Math.floor(duration / (1000 * 60 * 60));
  const minutes = Math.floor((duration / (1000 * 60)) % 60);
  const seconds = Math.floor((duration / 1000) % 60);
  return `${hours.toString().padStart(2, "0")}:${minutes
    .toString()
    .padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
};
