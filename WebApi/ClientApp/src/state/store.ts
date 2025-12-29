import { Tuple, configureStore } from "@reduxjs/toolkit";
import createSagaMiddleware from "redux-saga";
import rootReducer from "./reducers/rootReducer";
import rootSaga from "./sagas/rootSaga";
const sagaMiddleware = createSagaMiddleware();
export const store = configureStore({
    reducer: rootReducer,
    middleware: () => new Tuple(sagaMiddleware),
    // composeWithDevTools(applyMiddleware(sagaMiddleware))
});
sagaMiddleware.run(rootSaga)
export type AppDispatch = typeof store.dispatch;