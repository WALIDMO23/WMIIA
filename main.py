

import pandas as pd

# غير المسار حسب المكان اللي حفظت فيه الملفات فعليًا
df = pd.read_csv(r"C:\Users\free bytes\Downloads\archive\USvideos.csv.csv")

# عرض أول 5 صفوف للتأكد
print(df.head())
