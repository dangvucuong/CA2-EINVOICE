import { axiosClient } from "../api/axiosClient";
import { parseSoapResponse } from "./common";

const MAX_CHUNG_TU_DOWNLOAD = 20;

export async function inchuyendoiChungTu(
  machungtu: string | number,
  madonvi: string
): Promise<{ status?: string; message?: string; data?: unknown } | null> {
  const soap = `<?xml version="1.0" encoding="utf-8"?>
<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">
  <soap12:Body>
    <Inchuyendoi xmlns="http://tempuri.org/">
      <mact>${machungtu}</mact>
      <madonvi>${madonvi}</madonvi>
    </Inchuyendoi>
  </soap12:Body>
</soap12:Envelope>`;

  const res: string = await axiosClient.post(
    process.env.REACT_APP_API_CHUNG_TU as string,
    soap,
    {
      headers: {
        "Content-Type": "text/xml; charset=utf-8",
      },
    }
  );

  return parseSoapResponse(res);
}

export function getChungTuSiteBaseUrl(): string {
  const api = process.env.REACT_APP_API_CHUNG_TU || "";
  return api.replace(/\/service\.asmx$/i, "");
}

export function buildChungTuDownloadZipUrl(
  type: "pdf" | "xml",
  machungtuList: (string | number)[],
  madonvi: string
): string {
  const base = getChungTuSiteBaseUrl();
  const ids = machungtuList.map((x) => String(x)).join(",");
  const params = new URLSearchParams({
    type,
    madonvi,
    machungtu: ids,
  });
  return `${base}/DownloadZip.aspx?${params.toString()}`;
}

export { MAX_CHUNG_TU_DOWNLOAD };
