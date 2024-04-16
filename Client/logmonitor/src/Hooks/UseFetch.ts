interface fetchGetProps {
  Headers?: HeadersInit|undefined;
  url: string;
  returnType: "JSON" | "TEXT";
}

function useFetch() {
  const get = () => {};
  const getAsync = async ({ Headers = undefined, url, returnType }: fetchGetProps) => {
    return await fetch(url, { headers: Headers }).then(
      async (value: Response) => {
        if (returnType === "JSON") {
          return await value.json();
        }
        return value;
      }
    );
  };
  return { get, getAsync };
}

export default useFetch;
