import { FormControl, TextInput, TextInputProps } from "@primer/react";
import { useCommonContext } from "../../contexts/common";

export interface IMyTextInputProps extends TextInputProps {
  register?: any;
  errors?: any;
  validateMessage?: string;
  name?: string;
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  ref?: any;
}
const MyTextInput = (props: IMyTextInputProps) => {
  const { register, errors } = props;
  const { translate } = useCommonContext();

  return (
    <>
      {register && props.name && (
        <>
          <TextInput
            ref={props.ref}
            {...register(props.name, {
              required: {
                value: props.required,
                message: translate(props.validateMessage ?? ""),
              },
              minLength: {
                value: props.minLength,
                message: translate(props.validateMessage ?? ""),
              },
              maxLength: {
                value: props.maxLength,
                message: translate(props.validateMessage ?? ""),
              },
              pattern: {
                value: props.pattern,
                message: translate(props.validateMessage ?? ""),
              },
            })}
            name={props.name}
            {...props}
          />
          {errors && errors[props.name] && (
            <FormControl.Validation id={props.name} variant="error">
              <>{errors[props.name].message ?? ""}</>
            </FormControl.Validation>
          )}
        </>
      )}
      {(!register || !props.name) && (
        <TextInput {...props} onChange={props.onChange} />
      )}
    </>
  );
};

export default MyTextInput;
