import { Select } from '@primer/react';
import { useEffect } from 'react';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import { rootAction } from '../../state/actions/rootAction';
import { eReducerStatusBase } from '../../state/reducer-models/eReducerStatusBase';
interface ICmbCompanySizeProps {
    onValueChanged: (id: number) => void,
    value: number,
    maxWidth?: any,
    readonly?: boolean
}

const CmbCompanySize = (props: ICmbCompanySizeProps) => {
    const dispatch = useAppDispatch();
    const { status, companySizes } = useAppSelector(x => x.contact.companySizeReducer);
    useEffect(() => {
        if (status === eReducerStatusBase.is_not_initialization) {
            dispatch(rootAction.contact.companySizeAction.loadStart())
        }
    }, [status])

    return (
        <>
            <Select disabled={props.readonly} onChange={(e) => {
                props.onValueChanged(parseInt(e.target.value))

            }}
                value={props.value.toString()}
            >
                <Select.Option value="0"> --Chọn số lượng thành viên--</Select.Option>
                {companySizes.map(x => {
                    return (
                        <Select.Option value={x.id.toString()} key={x.id}
                        >{x.name}</Select.Option>
                    );
                })}
                {/* <Select.Option value="one">Choice one</Select.Option>
                <Select.Option value="two">Choice two</Select.Option>
                <Select.Option value="three">Choice three</Select.Option>
                <Select.Option value="four">Choice four</Select.Option>
                <Select.Option value="five">Choice five</Select.Option>
                <Select.Option value="six">Choice six</Select.Option> */}
            </Select>

        </>
    );
};

export default CmbCompanySize;