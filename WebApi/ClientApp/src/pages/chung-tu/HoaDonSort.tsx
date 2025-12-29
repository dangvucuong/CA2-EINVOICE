import {
  ClockIcon,
  NumberIcon,
  SortAscIcon,
  SortDescIcon,
} from "@primer/octicons-react";
import { ActionList, ActionMenu, Box } from "@primer/react";
import { eSortMode } from "../../models/commons/eSortMode";
interface IHoaDonSort {
  field: string;
  mode: eSortMode;
}
interface IHoaDonSortProps {
  sortBy: IHoaDonSort;
  onValueChanged: (sortBy: IHoaDonSort) => void;
}
const HoaDonSort = (props: IHoaDonSortProps) => {
  return (
    <Box sx={{ display: "flex", gap: 1, alignItems: "center" }}>
      <Box sx={{ color: "fg.muted" }}>Sắp xếp theo</Box>
      <ActionMenu>
        <ActionMenu.Button
          leadingVisual={
            props.sortBy.mode === eSortMode.DESC ? SortDescIcon : SortAscIcon
          }
        >
          <>
            {props.sortBy.field === "id" && <>Ngày tạo</>}
            {props.sortBy.field === "ma_so_hoa_don" && <>Số hóa đơn</>}
          </>
        </ActionMenu.Button>
        <ActionMenu.Overlay>
          <ActionList showDividers selectionVariant="multiple">
            <ActionList.Item
              onSelect={() => {
                props.onValueChanged({
                  ...props.sortBy,
                  field: "id",
                });
              }}
              selected={props.sortBy.field === "id"}
            >
              <ActionList.LeadingVisual>
                <ClockIcon />
              </ActionList.LeadingVisual>
              Ngày tạo
            </ActionList.Item>
            <ActionList.Item
              selected={props.sortBy.field === "ma_so_hoa_don"}
              onSelect={() => {
                props.onValueChanged({
                  ...props.sortBy,
                  field: "ma_so_hoa_don",
                });
              }}
            >
              <ActionList.LeadingVisual>
                <NumberIcon />
              </ActionList.LeadingVisual>
              Số hóa đơn
            </ActionList.Item>
            <ActionList.Divider></ActionList.Divider>
            <ActionList.Item
              selected={props.sortBy.mode === eSortMode.ASC}
              onSelect={() => {
                props.onValueChanged({
                  ...props.sortBy,
                  mode: eSortMode.ASC,
                });
              }}
            >
              <ActionList.LeadingVisual>
                <SortAscIcon />
              </ActionList.LeadingVisual>
              Tăng dần
            </ActionList.Item>
            <ActionList.Item
              selected={props.sortBy.mode === eSortMode.DESC}
              onSelect={() => {
                props.onValueChanged({
                  ...props.sortBy,
                  mode: eSortMode.DESC,
                });
              }}
            >
              <ActionList.LeadingVisual>
                <SortDescIcon />
              </ActionList.LeadingVisual>
              Giảm dần
            </ActionList.Item>
          </ActionList>
        </ActionMenu.Overlay>
      </ActionMenu>
    </Box>
  );
};

export default HoaDonSort;
