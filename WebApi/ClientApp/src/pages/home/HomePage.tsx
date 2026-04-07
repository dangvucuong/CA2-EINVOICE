import { useCallback, useRef, useState } from "react";
import { Helmet } from "react-helmet";
import WaterMarkTemplateSelection from "../../component-data/water-mark-selection";
import ThangSelection from "../../component-ui/thang-selection";

const HomePage = () => {
    const [isOpen, setIsOpen] = useState(false)
    const [secondOpen, setSecondOpen] = useState(false)
    const buttonRef = useRef<HTMLButtonElement>(null)
    const onDialogClose = useCallback(() => setIsOpen(false), [])
    const onSecondDialogClose = useCallback(() => setSecondOpen(false), [])
    const openSecondDialog = useCallback(() => setSecondOpen(true), [])
    return (
        <div>
            <Helmet>
                <title>Trang chủ</title>
            </Helmet>
            {/* <WaterMarkTemplateSelection
                onSelectionChanged={() => { }}
            /> */}
            <ThangSelection value={1} onValueChanged={()=>{}} />

            {/* <KySoModal base64=""/> */}
        </div>
    );
};

export default HomePage;