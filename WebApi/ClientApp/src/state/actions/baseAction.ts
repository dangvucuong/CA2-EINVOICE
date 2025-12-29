import { IActionTypeBase } from "../action-types/IActionTypeBase";

export function baseAction<X, Y>(type: X, payload: Y): IActionTypeBase<X, Y> {
    return {
        type: type,
        payload: payload 
    }
}

