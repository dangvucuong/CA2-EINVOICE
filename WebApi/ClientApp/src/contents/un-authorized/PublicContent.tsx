import { Box } from '@primer/react';
import { Route, Switch } from 'react-router-dom';
import publicRouter from '../../layouts/page/publicRouter';
import UnAuthorizedContent from './UnAuthorizedContent';
const PublicContent = () => {

    
    return (
        <Box
        >
            <Switch>
                {publicRouter.map(({ path, component }) => (
                    <Route
                        strict
                        key={path}
                        path={path}
                        component={component}
                    />
                ))}
                <Route component={UnAuthorizedContent} />
            </Switch>
        </Box>

    );
};

export default PublicContent;