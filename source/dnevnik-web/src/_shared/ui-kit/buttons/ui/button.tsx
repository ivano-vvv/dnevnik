import { FC, PropsWithChildren } from "react";
import classnames from 'classnames';

import s from './button.module.scss';


export type ButtonProps = PropsWithChildren & {
    className?: string;
    disabled?: boolean;
}

export const Button: FC<ButtonProps> = (props) => {
    return (
        <button {...props} className={classnames(s.self, props.className)}>
            {props.children}
        </button>
    );
};
