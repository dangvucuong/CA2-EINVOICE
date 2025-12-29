import { ActionList, ActionMenu, Box, Button } from '@primer/react';
import { useAppSelector } from '../../hooks/useAppSelector';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { rootAction } from '../../state/actions/rootAction';
const ViIcon = () => {
    return (
        <img alt='vi' src='../../../images/vi.png' style={{
            height: "16px"
        }} />
    )
}
const EnIcon = () => {
    return (
        <img alt='vi' src='../../../images/en.png' style={{
            height: "16px"
        }} />
    )
}
const LanguageSection = () => {
    const { lan } = useAppSelector(x => x.common.localizedResourceReducer)
    const dispatch = useAppDispatch();

    return (
        <Box>
            <ActionMenu>
                <ActionMenu.Button leadingVisual={(lan === "en" ? EnIcon : ViIcon)}>
                    {lan === "en" ? "English" : "Tiếng Việt"}
                </ActionMenu.Button>
                <ActionMenu.Overlay width="auto">
                    <ActionList showDividers selectionVariant='single'>
                        <ActionList.Item
                            onSelect={() => {
                                dispatch(rootAction.common.localizedResourceAction.changeLanguage("vi"))
                            }}
                            selected={lan === "vi"}>
                            <ActionList.LeadingVisual>
                                <ViIcon />
                            </ActionList.LeadingVisual>
                            Tiếng Việt
                        </ActionList.Item>
                        <ActionList.Item
                            onSelect={() => {
                                dispatch(rootAction.common.localizedResourceAction.changeLanguage("en"))
                            }}
                            selected={lan === "en"}>
                            <ActionList.LeadingVisual>
                                <EnIcon />
                            </ActionList.LeadingVisual>
                            English
                        </ActionList.Item>

                    </ActionList>
                </ActionMenu.Overlay>
            </ActionMenu>
        </Box>
    );
};

export default LanguageSection;