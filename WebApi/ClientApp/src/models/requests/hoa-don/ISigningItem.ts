export type ISigningItem = {
  hoaDonId: number;
  sessionId: string;
  hashBase64: string;
  signatureValue?: string;

  status:
    | "preparing"
    | "prepared"
    | "signing"
    | "signed"
    | "finalizing"
    | "completed"
    | "error";

  error?: string;
};