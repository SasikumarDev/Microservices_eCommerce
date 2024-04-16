export function formatDateTime(format: "dd-MMM-yyyy hh:mm", date: Date) {
  if (format !== "dd-MMM-yyyy hh:mm") return null;
  const monthFormat = new Intl.DateTimeFormat("en-us", { month: "short" });
  const dayFormat = new Intl.DateTimeFormat("en-us", { day: "2-digit" });
  const yearFormat = new Intl.DateTimeFormat("en-us", { year: "numeric" });
  return `${dayFormat.format(date)}-${monthFormat.format(
    date
  )}-${yearFormat.format(date)}`;
}
