import { Textarea } from "@primer/react";
import React, { useLayoutEffect, useRef } from "react";

function AutoResizeText({
  value,
  onChange,
  minHeight = 32,
  sx = {},
}: {
  value: string;
  onChange: (v: string) => void;
  minHeight?: number;
  sx?: any;
}) {
  const ref = useRef<HTMLTextAreaElement | null>(null);

  useLayoutEffect(() => {
    if (ref.current) {
      ref.current.style.height = "0px";
      ref.current.style.height =
        Math.max(ref.current.scrollHeight, minHeight) + "px";
    }
  }, [value, minHeight]);
  return (
    <Textarea
      ref={ref}
      value={value ?? ""}
      resize="vertical"
      rows={1}
      className="noborder"
      sx={{
        resize: "none",
        overflow: "hidden",
        width: "100%",
        lineHeight: "20px",
        whiteSpace: "pre-wrap",
        wordBreak: "break-word",
        minHeight: `${minHeight}px`,
        p: 0,
        background: "transparent",
        ...sx,
      }}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}

export default AutoResizeText;
