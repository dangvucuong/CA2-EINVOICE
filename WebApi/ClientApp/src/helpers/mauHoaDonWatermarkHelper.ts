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
  const logoUrl = options.logoUrl?.replace(/\\/g, "/") ?? "";
  const paramOpacity = `${1 - options.opacity / 100}`;

  result = result.replace(/\{paramLogo\}/g, logoUrl);

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

  return result;
};
