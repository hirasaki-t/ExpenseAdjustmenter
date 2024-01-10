export type SundryExpenseListData = {
    id: string;
    date: Date;
    expenseTypeId: string;
    details: string | null;    
    participationNumber: number;
    receiptId: string | null;
    submissionMethod: string | null;
    amount: number;
    reviewerId: string | null;
    comment: string | null;
    status: string | null;
}