import React, {useContext} from "react";

import {FrontendContext} from "../Internals/ContextStore";

/**
 * Contact component that displays contact information.
 *
 * This component utilizes the FrontendContext to retrieve necessary
 * data or context values for rendering the contact section.
 *
 * The Contact component contains a simple layout with a heading
 * and a paragraph of placeholder text describing various stylistic
 * and structural elements.
 *
 * @component
 * @returns {React.FC} A functional component rendering the contact section.
 */
const Contact: React.FC = () => {
    const frontendContext = useContext(FrontendContext);
    return (
        <div>
            <h1>Contact</h1>
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
};

export default Contact;
