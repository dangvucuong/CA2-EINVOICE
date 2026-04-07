import { Box } from "@primer/react";
import React from "react";
import Text from "../text";
import { BetterSystemStyleObject } from "@primer/react/lib/sx";
interface IPaperFormGroupProps {
  label: string;
  children: React.ReactNode;
  isHideBorder?: boolean;
  style?: BetterSystemStyleObject;
}
const PaperFormGroup = (props: IPaperFormGroupProps) => {
  return (
    <Box
      sx={{
        display: "flex",
        borderTopStyle: "solid",
        borderTopWidth: props.isHideBorder ? 0 : 1,
        borderTopColor: "border.default",
        mt: 4,
        pt: 4,
        ...props.style,
      }}
    >
      <Box
        sx={{
          width: "250px",
          pr: 3,
        }}
      >
        <Text
          text={props.label}
          sx={{
            fontWeight: "bold",
            color: "fg.muted",
            whiteSpace: "break-spaces",
          }}
        />
      </Box>
      <Box
        sx={{
          flex: 1,
        }}
      >
        {props.children}
      </Box>
    </Box>
  );
};

export default PaperFormGroup;
