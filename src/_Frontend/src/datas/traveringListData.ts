export type TraveringListData = {
    id: string;
    date: Date;
    startSection: string | null;
    endSection: string | null;
    categoryId: string;    
    workName: string;
    remarks: string | null;
    amount: number;
    receiptId: string | null;
    submissionMethod: string | null,
    reviewerId: string | null;
    comment: string | null;
    status: string | null;
}