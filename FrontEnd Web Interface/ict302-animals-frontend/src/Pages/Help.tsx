import React, {useContext} from 'react';

import {FrontendContext} from "../Internals/ContextStore";

/**
 * Help component.
 *
 * This is a React functional component that fetches and displays help
 * related information. It utilizes the FrontendContext to access the
 * necessary data and context for rendering.
 *
 * The component currently displays a heading "Help" and a placeholder
 * paragraph with example text.
 *
 * @constant
 * @type {React.FC}
 */
const Help: React.FC = () => {
    const frontendContext = useContext(FrontendContext);

    return (
        <div>
            <h1>Help</h1>
            <p>
                Pellentesque habitant morbi tristique senectus et netus et malesuada
                fames ac turpis egestas. Vestibulum tortor quam, feugiat vitae,
                ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam
                egestas semper. Aenean ultricies mi vitae est. Mauris placerat eleifend
                leo. Quisque sit amet est et sapien ullamcorper pharetra. Vestibulum
                erat wisi, condimentum sed, commodo vitae, ornare sit amet, wisi. Aenean
                fermentum, elit eget tincidunt condimentum, eros ipsum rutrum orci,
                sagittis tempus lacus enim ac dui. Donec non enim in turpis pulvinar
                facilisis. Ut felis. Praesent dapibus, neque id cursus faucibus, tortor
                neque egestas augue, eu vulputate magna eros eu erat. Aliquam erat
                volutpat. Nam dui mi, tincidunt quis, accumsan porttitor, facilisis
                luctus, metus
            </p>
        </div>
    );
}

export default Help;