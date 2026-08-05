const MAX_CHUNG_TU_DOWNLOAD = 20;

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
