import Big from "big.js";

export interface ITienThueTheoMuc {
  thanh_tien_tinh_thue: number;
  phan_tram: number;
}

export const getPhanTramThue = (thueSuat: string): number => {
  const value = parseFloat(
    (thueSuat ?? "").replace("KHAC:", "").replace("%", "").trim(),
  );
  return isNaN(value) ? 0 : parseFloat(new Big(value).round(2, Big.roundHalfUp).toString());
};

export const lamTronVnd = (value: number): number =>
  parseFloat(new Big(value ?? 0).round(0, Big.roundHalfUp).toString());

const floorBig = (value: Big): Big => {
  const truncated = value.round(0, Big.roundDown);
  return value.lt(truncated) ? truncated.minus(1) : truncated;
};

/**
 * Tiền thuế VND theo từng mức: tổng tiền thuế chỉ làm tròn một lần từ tổng chưa làm tròn,
 * phần chênh do làm tròn chia cho các mức có phần lẻ lớn nhất (bằng nhau thì ưu tiên thuế suất cao, rồi tiền tính thuế lớn),
 * nên tổng tiền thuế các mức luôn bằng Round(tổng tiền thuế chưa làm tròn).
 * Phải giữ giống hệt HoaDonTienThueHelper.PhanBoTienThueVnd ở Service/Helper/HoaDonTienThueHelper.cs.
 */
export const phanBoTienThueVnd = (items: ITienThueTheoMuc[]): number[] => {
  const exacts = items.map((x) =>
    new Big(x.thanh_tien_tinh_thue ?? 0).times(x.phan_tram ?? 0).div(100),
  );
  const sign = exacts.every((x) => x.lte(0)) ? -1 : 1;
  const values = exacts.map((x) => x.times(sign));

  const target = values
    .reduce((a, b) => a.plus(b), new Big(0))
    .round(0, Big.roundHalfUp);
  const result = values.map(floorBig);
  let k = Number(target.minus(result.reduce((a, b) => a.plus(b), new Big(0))).toString());

  const order = values
    .map((_, i) => i)
    .sort((a, b) => {
      const remCompare = values[b].minus(result[b]).cmp(values[a].minus(result[a]));
      if (remCompare !== 0) return remCompare;
      if (items[b].phan_tram !== items[a].phan_tram) return items[b].phan_tram - items[a].phan_tram;
      const baseCompare = new Big(items[b].thanh_tien_tinh_thue ?? 0).abs()
        .cmp(new Big(items[a].thanh_tien_tinh_thue ?? 0).abs());
      if (baseCompare !== 0) return baseCompare;
      return a - b;
    });
  for (let j = 0; j < order.length && k > 0; j++, k--) {
    result[order[j]] = result[order[j]].plus(1);
  }

  return result.map((x) => Number(x.times(sign).toString()) + 0);
};
