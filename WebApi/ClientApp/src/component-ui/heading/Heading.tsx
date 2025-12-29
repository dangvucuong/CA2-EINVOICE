import { Heading, SxProp } from '@primer/react';
import { eSize } from '../../models/commons/eSize';
import Text from '../text';
interface IHeadingProps extends SxProp {
    size?: eSize,
    text: string
}
const MyHeading = (props: IHeadingProps) => {
    return (
        <Heading sx={{
            fontSize: props.size == eSize.smalll ? "13px" : (props.size == eSize.medium ? "16px" : "24px"),
            ...props.sx
        }}>
            <Text text={props.text} />
        </Heading>
    );
};

export default MyHeading;