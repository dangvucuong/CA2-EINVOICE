import React from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';
import appRouter from './appRouter';

const PageContent = () => {
    // console.log({
    //     page: "PageContent",
    //     path: appRouter.map(x=>x.path)
    // });
    const defaultPage = () => {
        return "/dashboard";
    }
    return (
        <React.Fragment>
            <Switch>
                {appRouter.map(({ path, component }) => (
                    <Route
                        strict
                        key={path}
                        path={path}
                        exact
                        component={component}
                    />
                ))}
                <Redirect to={defaultPage()} />
            </Switch>
        </React.Fragment>
    );
};

export default PageContent;