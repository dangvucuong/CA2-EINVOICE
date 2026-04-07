import { ActionList, Link, Spinner } from '@primer/react';
import { ComponentProps, useState } from 'react';
interface ILinkS3Props extends ComponentProps<typeof Link> {
    url: string
}
const LinkS3 = (props: ILinkS3Props) => {
    const { url } = props;
    const [expireLink, setExpireLink] = useState(props.url);
    const [isLoading, setIsLoading] = useState(false);
    const [isMouseEnter, setIsMouseEnter] = useState(false);

    // useEffect(() => {
    //     if (!expireLink && url && isMouseEnter) {
    //         handleGetLink();
    //     }
    // }, [url, expireLink, isMouseEnter])
    // const handleGetLink = async () => {
    //     setIsLoading(true)
    //     const res = await uploadApi.createLink(props.url)
    //     setIsLoading(false)
    //     if (res.is_success) {
    //         setExpireLink(res.data?.url ?? "")
    //     } else {
    //         NotifyHelper.Error("Error");
    //     }
    // }
    return (
        <Link href={expireLink}
            target="_blank"
            sx={{
                ...props.sx,
                display: "flex"
            }}
            onMouseEnter={() => {
                if (!isMouseEnter) {
                    setIsMouseEnter(true)
                }
            }}
        >
            {props.children}
            {(isLoading) &&
                <ActionList.LeadingVisual>
                    <Spinner size='small' sx={{ ml: 2 }} />
                </ActionList.LeadingVisual>
            }
        </Link>
    );
};

export default LinkS3;