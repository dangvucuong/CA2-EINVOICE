import type { TypedUseSelectorHook } from "react-redux";
import { useSelector } from "react-redux";
import { RootState } from "../state/reducers/rootReducer";
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;