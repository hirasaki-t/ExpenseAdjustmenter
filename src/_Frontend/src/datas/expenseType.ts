export type ExpenseType = {
    id: string;
    name: string;
    details: string | null;
    isReceipt: boolean;
    isActive: boolean;
}