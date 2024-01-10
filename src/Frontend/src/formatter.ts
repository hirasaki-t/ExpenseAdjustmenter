const yearMonthFormatter = new Intl.DateTimeFormat("ja-JP", { year: "numeric", month: "long" });
const yearMonthFormatter_splitSlash = new Intl.DateTimeFormat("ja-JP", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
});
export function formatDateToYearMonth(date?: Date, splitSlash?: boolean) {
  if (!date) return null;
  return splitSlash ? yearMonthFormatter_splitSlash.format(date) : yearMonthFormatter.format(date);
}

const dateFormatter = new Intl.DateTimeFormat("ja-JP", { year: "numeric", month: "long", day: "numeric" });
export function formatDate(date?: Date) {
  if (!date) return null;
  return dateFormatter.format(date);
}

const numberFormatter = new Intl.NumberFormat();
export function formatNumber(number?: number) {
  if (number === undefined) return;
  return numberFormatter.format(number);
}
