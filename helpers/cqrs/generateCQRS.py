import os
import re

TEMPLATE_HANDLER = '''using MediatR;

namespace {Namespace};

public class {Entity}Handler : IRequestHandler<{Entity}, Guid>
{
    public Task<Guid> Handle({Entity} request, CancellationToken cancellationToken)
    {
        // TODO: сделать!
        throw new NotImplementedException();
    }
}
'''

TEMPLATE_VALIDATOR = '''using FluentValidation;

namespace {Namespace};

public class {Entity}Validator : AbstractValidator<{Entity}>
{
    public {Entity}Validator()
    {
        // TODO: сделать!
        throw new NotImplementedException();
    }
}
'''

def find_command_or_query_file():
    """Ищет первый .cs файл, заканчивающийся на Command.cs или Query.cs в текущей директории."""
    for filename in os.listdir("."):
        if filename.endswith("Command.cs") or filename.endswith("Query.cs"):
            return os.path.abspath(filename)
    raise FileNotFoundError("❌ Не найден файл *Command.cs или *Query.cs в текущей директории.")

def extract_namespace_from_file(filepath):
    """
    Извлекает namespace из C# файла:
    - поддерживает file-scoped (с `;`)
    - поддерживает block-scoped (с `{`)
    - не зависит от позиции в файле
    - обрабатывает UTF-8 BOM
    """
    with open(filepath, encoding="utf-8-sig") as f:
        content = f.read()

    match = re.search(r'^\s*namespace\s+([\w\.]+)', content, re.MULTILINE)
    if match:
        return match.group(1)

    raise ValueError(f"❌ Не найден namespace в файле: {filepath}")

def extract_entity_name(filepath):
    """Извлекает имя команды/запроса (например, CreateAccountCommand)."""
    filename = os.path.basename(filepath)
    match = re.match(r'^(.*)(Command|Query)\.cs$', filename)
    if not match:
        raise ValueError(f"❌ Невозможно извлечь имя команды из имени файла: {filename}")
    return match.group(1) + match.group(2)  # Возвращаем с суффиксом

def generate_files(entity_name, namespace, target_dir):
    os.makedirs(target_dir, exist_ok=True)

    files = {
        f"{entity_name}Handler.cs": TEMPLATE_HANDLER,
        f"{entity_name}Validator.cs": TEMPLATE_VALIDATOR,
    }

    for filename, template in files.items():
        content = template.replace("{Entity}", entity_name).replace("{Namespace}", namespace)
        file_path = filename
        with open(file_path, "w", encoding="utf-8") as f:
            f.write(content)

    print(f"✅ Файлы CQRS для сущности '{entity_name}' созданы в папке '{target_dir}'.")

if __name__ == "__main__":
    try:
        cs_path = find_command_or_query_file()
        entity = extract_entity_name(cs_path)
        namespace = extract_namespace_from_file(cs_path)
        target_dir = os.path.join(os.path.dirname(cs_path), entity)
        generate_files(entity, namespace, target_dir)
    except Exception as e:
        print(str(e))
        exit(1)
