export function toDictionary<T, TValue>(array: T[], keyPicker: (arg: T) => string, valuePicker: (arg: T) => TValue) {
    return array.reduce((x, y) => {
        x[keyPicker(y)] = valuePicker(y);
        return x;
    }, {} as { [key: string]: TValue });
}

export function distinct<T>(array: T[]){
    if (array.length === 0) return undefined;
    const set = new Set();
    for (const item of array) set.add(item);
    return Array.from(set);
}

export function dateDistinct(array: Date[]){
    if (array.length === 0) return undefined;
    const set = new Set();
    for (const item of array) set.add(item.getTime());
    return Array.from(set);
}

export function min<T>(array: T[], targetPicker: (arg: T) => number | Date) {
    if (array.length === 0) return undefined;
    return array.map((x) => targetPicker(x)).reduce((a, b) => (a < b ? a : b));
}

export function max<T>(array: T[], targetPicker: (arg: T) => number | Date) {
    if (array.length === 0) return undefined;
    return array.map((x) => targetPicker(x)).reduce((a, b) => (a > b ? a : b));
}

export function minBy<T>(array: T[], targetPicker: (arg: T) => number | Date) {
    const minValue = min(array, targetPicker);
    return array.find((x) => targetPicker(x) === minValue);
}

export function maxBy<T>(array: T[], targetPicker: (arg: T) => number | Date) {
    const maxValue = max(array, targetPicker);
    return array.find((x) => targetPicker(x) === maxValue);
}

export function groupBy<T, TKey>(array: T[], keyPicker: (arg: T) => TKey) {
    const keys = Array.from(new Set(array.map((x) => keyPicker(x))));
    return keys.map((x) => ({ key: x, values: array.filter((y) => keyPicker(y) === x) }));
}

export function sum(array: number[]) {
    return array.reduce((sum, element) => sum + element, 0);
}