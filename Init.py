import os
import fileinput
import glob

old_name = "Toolkit" + "Boilerplate"
path = input("Path (./): ")
new_name = input("New name: ")

filenames = glob.glob(("./" if path == "" else path) + '**', recursive=True)

for filename in filenames:
    if os.path.isdir(filename) or (os.sep + ".") in filename:
        continue
    try:
        for line in fileinput.input(filename, inplace=1):
            print(line.replace(old_name, new_name), end ='')
        os.rename(filename, os.sep.join(filename.split(os.sep)[:-1]) + os.sep + filename.split(os.sep)[-1].replace(old_name, new_name))
        print(filename)
    except:
        pass