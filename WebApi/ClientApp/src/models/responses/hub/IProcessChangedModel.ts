export interface IProcessChangedModel<T> {
    user_id: string;
    is_finished: boolean;
    processStatus: IProcessStatusRespone<T>;
}

export interface IProcessStatusRespone<T> {
    progress_id: string;
    steps: IProcessStepRespone<T>[];
}

export interface IProcessStepRespone<T> {
    id: number;
    name: string;
    data: T;
}

export interface IProcessStepDataBase {
    total: number;
    success: number;
    error: number;
    is_done: boolean;
    is_processing: boolean;
}