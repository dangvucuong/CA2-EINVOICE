interface IPageCaptionProps {
    caption: string
}
const PageCaption = (props: IPageCaptionProps) => {
    return (
        <div style={{
            fontSize: "18px",
            fontWeight:600
        }}>
            {props.caption}
        </div>
    );
};

export default PageCaption;