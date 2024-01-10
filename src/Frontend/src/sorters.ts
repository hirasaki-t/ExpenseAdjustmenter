import type { IdObject } from "./types";

/** IDによるソートを行う */
export function idSorter(a: IdObject, b: IdObject) {
  if (a.id === b.id) return 0;
  return a.id < b.id ? -1 : 1;
}

/** 日付によるソートを行う */
export function dateSorter(a?: Date, b?: Date) {
  if (a === b) return 0;
  if (!a) return -1;
  if (!b) return 1;
  return a < b ? -1 : 1;
}
