import { FormRules } from "element-plus";

export const sundryExpenseRules: FormRules = {
    date: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    expenseTypeId: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    details: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    participationNumber: {
        required: true,
        pattern: /[0-9]/,
        message: "半角数値で必須項目です",
        trigger: "blur",
    },
    amount: {
        required: true,
        pattern: /[0-9]/,
        message: "半角数値で必須項目です",
        trigger: "blur",
    },
    submissionMethod: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
    receipt: {
        required: true,
        message: "必須項目です",
        trigger: "blur",
    },
};