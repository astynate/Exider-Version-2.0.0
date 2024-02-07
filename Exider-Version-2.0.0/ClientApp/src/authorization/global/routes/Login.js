﻿import PasswordField from '../../components/fields/password/PasswordField';
import TextField from '../../components/fields/text/TextField';
import SubmitButton from '../../components/fields/button/SubmitButton';
import logo from '../../global/images/logo.png';
import CheckBox from '../../components/fields/check-box/CheckBox';

const Login = () => {

    return (

        <>

            <img src={logo} className="logo" draggable="false" />
            <h1>Login to <span>Exider</span></h1>
            <TextField name="email" placeholder="Email or nickname" />
            <PasswordField />
            <SubmitButton value="Login" isDisabled={true} />
            <CheckBox name="Remember me" />

        </>

    );

};

export default Login;