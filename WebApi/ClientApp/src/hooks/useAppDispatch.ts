import { useDispatch } from "react-redux";
import { AppDispatch } from "../state/store";

// Sử dụng 2 hook này để get state và dispatch actions.
export const useAppDispatch: () => AppDispatch = useDispatch;
