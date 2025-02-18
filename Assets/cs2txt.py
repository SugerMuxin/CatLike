import os
import sys
import chardet

def detect_file_encoding(file_path):
    with open(file_path, "rb") as f:
        raw_data = f.read()
    result = chardet.detect(raw_data)
    return result["encoding"]

def process_directory(root_dir):
    for root, dirs, files in os.walk(root_dir):
        for file in files:
            if file.endswith(".cs"):
                cs_path = os.path.join(root, file)
                txt_filename = os.path.splitext(file)[0] + ".txt"
                txt_path = os.path.join(root, txt_filename)
                
                try:
                    # 检测文件编码
                    encoding = detect_file_encoding(cs_path)
                    print(f"Detected encoding for {cs_path}: {encoding}")
                    
                    # 使用检测到的编码读取文件
                    with open(cs_path, "r", encoding=encoding) as cs_file:
                        content = cs_file.read()
                    
                    # 写入 UTF-8 编码的 txt 文件
                    with open(txt_path, "w", encoding="utf-8") as txt_file:
                        txt_file.write(content)
                    
                    print(f"Successfully created: {txt_path}")
                except Exception as e:
                    print(f"Error processing {cs_path}: {str(e)}")

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python script.py <target_directory>")
        sys.exit(1)
    
    target_dir = sys.argv[1]
    if not os.path.isdir(target_dir):
        print(f"Error: {target_dir} is not a valid directory")
        sys.exit(1)
    
    process_directory(target_dir)