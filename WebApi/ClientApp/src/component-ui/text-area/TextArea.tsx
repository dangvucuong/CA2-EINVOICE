import { FormControl, Textarea, TextareaProps } from "@primer/react";
import { useCommonContext } from "../../contexts/common";

interface IMyTextAreaProps extends TextareaProps {
  register?: any;
  errors?: any;
  validateMessage?: string;
  name?: string;
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  resize?: "none" | "both" | "horizontal" | "vertical";
  sx?: any;
}
const MyTextArea = (props: IMyTextAreaProps) => {
  const { register, errors, resize, sx } = props;
  const { translate } = useCommonContext();

  return (
    <>
      {register && props.name && (
        <>
          <Textarea
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
            })}
            name={props.name}
            resize={resize ?? "none"}
            sx={sx}
            // sx={props.sx}
            {...props}
          />
          {errors && errors[props.name] && (
            <FormControl.Validation id={props.name} variant="error">
              <>{errors[props.name].message ?? ""}</>
            </FormControl.Validation>
          )}
        </>
      )}
      {(!register || !props.name) && <Textarea sx={props.sx} />}
    </>
  );
};

export default MyTextArea;
