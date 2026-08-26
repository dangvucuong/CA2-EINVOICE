const TRANSPARENT_LOGO_DATA_URI =
  "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

const resolveLogoUrl = (logoUrl?: string) => {
  const url = logoUrl?.replace(/\\/g, "/")?.trim() ?? "";
  return url || TRANSPARENT_LOGO_DATA_URI;
};

const normalizeEmptyLogoImages = (html: string) => {
  return html
    .replace(
      /(id\s*=\s*["']imgSample["'][^>]*?\bsrc\s*=\s*["'])(?:paramLogo)?(["'])/gi,
      `$1${TRANSPARENT_LOGO_DATA_URI}$2`,
    )
    .replace(
      /(\bsrc\s*=\s*["'])(?:paramLogo)?(["'][^>]*?\bid\s*=\s*["']imgSample["'])/gi,
      `$1${TRANSPARENT_LOGO_DATA_URI}$2`,
    );
};

export const applyMauHoaDonWatermarkToHtml = (
  html: string,
  options: {
    watermarkUrl?: string;
    opacity: number;
    isShowWatterMarkInnerTable: boolean;
    logoUrl?: string;
  },
) => {
  let result = html;
  const watermarkUrl = options.watermarkUrl?.replace(/\\/g, "/") ?? "";
  const logoUrl = resolveLogoUrl(options.logoUrl);
  const paramOpacity = `${1 - options.opacity / 100}`;

  result = result.replace(/\{paramLogo\}/g, logoUrl);
  result = result.replace(/paramLogo/g, logoUrl);

  if (options.isShowWatterMarkInnerTable) {
    result = result.replace(/\{paramWaterMark\}/g, "");
    if (watermarkUrl) {
      result = result.replace(/paramWaterMarkTable;/g, watermarkUrl);
      const tableBgStyle = `background-image:url('${watermarkUrl}');background-size:cover;background-position:center;background-repeat:no-repeat;background-color:hsla(0,0%,100%,${paramOpacity});background-blend-mode:overlay;`;
      result = result.replace(/paramTableBG/g, tableBgStyle);
    } else {
      result = result.replace(/paramWaterMarkTable;/g, "");
      result = result.replace(/paramTableBG/g, "");
    }
  } else {
    result = result.replace(/\{paramWaterMark\}/g, watermarkUrl);
    result = result.replace(/paramWaterMarkTable;/g, "");
    result = result.replace(/paramTableBG/g, "");
  }

  result = result.replace(/paramOpacity;/g, paramOpacity);

  if (options.isShowWatterMarkInnerTable && watermarkUrl) {
    const innerTableCss =
      '<style>table.inner-watermark-table td,table.inner-watermark-table th,table[style*="background-image"] td,table[style*="background-image"] th{background-color:transparent !important;}</style>';
    if (!result.includes("inner-watermark-table") && result.includes("</head>")) {
      result = result.replace("</head>", `${innerTableCss}</head>`);
    }

    result = result.replace(
      /<div style="background:url\('([^']*)'\);background-color:\s*hsla\(0,0%,100%,([^)]+)\);background-blend-mode:\s*overlay;">\s*<table style="/gi,
      (_match, url, opacity) =>
        `<div><table class="inner-watermark-table" style="background-image:url('${url}');background-size:cover;background-position:center;background-repeat:no-repeat;background-color:hsla(0,0%,100%,${opacity});background-blend-mode:overlay;`,
    );
  }

  return normalizeEmptyLogoImages(result);
};
